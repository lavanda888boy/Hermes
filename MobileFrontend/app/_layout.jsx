import "../global.css";
import * as SplashScreen from "expo-splash-screen";
import { useFonts } from "expo-font";
import { ApiContextProvider } from "../config/apiContext.js";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useEffect, useState } from "react";
import { useRouter, Slot, useRootNavigationState } from "expo-router";
import * as Location from "expo-location";
import * as TaskManager from "expo-task-manager";

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
        console.log(error)
      }
    };

    requestLocationPermissions();
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
        router.replace("/");
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
  console.log("hi")
  if (error) {
    console.log('LOCATION_TRACKING task ERROR:', error);
    return;
  }

  if (data) {
    const { locations } = data;
    let lat = locations[0].coords.latitude;
    let long = locations[0].coords.longitude;

    console.log(
      `${new Date(Date.now()).toLocaleString()}: ${lat},${long}`
    );
  }
});
