import "../global.css";
import * as SplashScreen from "expo-splash-screen";
import { useFonts } from "expo-font";
import { ApiContextProvider } from "../config/apiContext.js";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useEffect } from "react";
import { useRouter, Slot } from "expo-router";
import * as Location from "expo-location";
import * as TaskManager from "expo-task-manager";
import { gpsTrackingApi } from "../config/axios.js";
import * as Notifications from "expo-notifications";
import { emitNotificationUpdate } from "../config/notificationEmitter.js";

SplashScreen.preventAutoHideAsync();

const App = () => {
  const router = useRouter();

  useEffect(() => {
    const requestLocationPermissions = async () => {
      let foregroundPermissionsStatus = await Location.requestForegroundPermissionsAsync();

      if (foregroundPermissionsStatus.status !== "granted") {
        alert("You need to enable location permissions to use the app.");
        return;
      }

      let backgroundPermissionsStatus = await Location.requestBackgroundPermissionsAsync();

      if (backgroundPermissionsStatus.status !== "granted") {
        alert("You also need to enable background location permissions.");
      }

      try {
        await Location.startLocationUpdatesAsync(process.env.LOCATION_TRACKING_TASK, {
          accuracy: Location.Accuracy.Highest,
          timeInterval: parseInt(process.env.LOCATION_TRACKING_TIME_INTERVAL),
          distanceInterval: parseInt(process.env.LOCATION_TRACKING_DISTANCE_INTERVAL),
        });
      } catch (error) {
        console.error(error)
      }
    };

    requestLocationPermissions();
  }, []);

  useEffect(() => {
    const handleNotifications = async () => {
      Notifications.setNotificationHandler({
        handleNotification: async () => ({
          shouldShowAlert: true,
          shouldPlaySound: true,
          shouldSetBadge: false,
        }),
      });

      const saveNotification = async (notificationData) => {
        let notifications = await AsyncStorage.getItem("notifications");
        notifications = notifications ? JSON.parse(notifications) : [];

        if (notifications.length >= parseInt(process.env.NOTIFICATION_BUFFER_SIZE ?? "10")) {
          notifications.pop();
        }

        notifications.unshift(notificationData);
        await AsyncStorage.setItem("notifications", JSON.stringify(notifications));

        emitNotificationUpdate(notifications);
      };


      Notifications.addNotificationReceivedListener(async (notification) => {
        const data = notification.request.content.data;
        await saveNotification(data);
      });

      Notifications.addNotificationResponseReceivedListener(async (response) => {
        const data = response.notification.request.content.data;
        await saveNotification(data);

        router.replace("/home");
      });
    }

    handleNotifications();
  }, []);

  const [isLoaded] = useFonts({
    "OpenSans-Italic": require("../assets/fonts/OpenSans-Italic.ttf"),
    "OpenSans-Light": require("../assets/fonts/OpenSans-Light.ttf"),
    "OpenSans-Regular": require("../assets/fonts/OpenSans-Regular.ttf"),
    "OpenSans-SemiBold": require("../assets/fonts/OpenSans-SemiBold.ttf"),
  });

  if (isLoaded) {
    SplashScreen.hideAsync();
  }

  useEffect(() => {
    const checkDeviceRegistration = async () => {
      const deviceRegistered = await AsyncStorage.getItem("deviceRegistered");

      if (deviceRegistered === "true") {
        router.replace("/home");
      } else {
        router.replace("/init");
      }
    };

    checkDeviceRegistration();
  }, []);

  return (
    <ApiContextProvider>
      <Slot />
    </ApiContextProvider>
  );
}

export default App;

TaskManager.defineTask(process.env.LOCATION_TRACKING_TASK, async ({ data, error }) => {
  if (error) {
    console.log('LOCATION_TRACKING task ERROR:', error);
    return;
  }

  if (data) {
    const { locations } = data;

    try {
      const fcmToken = await AsyncStorage.getItem("fcmToken");

      await gpsTrackingApi.post(`/${fcmToken}`, {
        "longitude": locations[0].coords.longitude,
        "latitude": locations[0].coords.latitude
      });
    } catch (error) {
      console.error(error);
    }
  }
});
