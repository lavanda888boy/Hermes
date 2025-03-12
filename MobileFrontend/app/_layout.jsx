import "../global.css";
import * as SplashScreen from "expo-splash-screen";
import { useFonts } from "expo-font";
import { ApiContextProvider } from "../config/apiContext.js";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useEffect } from "react";
import { useRouter, Slot } from "expo-router";

SplashScreen.preventAutoHideAsync();

export default function App() {
  const router = useRouter();

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
