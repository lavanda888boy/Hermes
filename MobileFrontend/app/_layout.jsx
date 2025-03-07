import "../global.css";
import InitScreen from "./index.jsx";
import * as SplashScreen from "expo-splash-screen";
import { useFonts } from "expo-font";
import { SafeAreaView } from "react-native-safe-area-context";
import { FCMProvider } from "../config/FCMContext.js";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useState, useEffect } from "react";

SplashScreen.preventAutoHideAsync();

export default function App() {
  const [isLoaded] = useFonts({
    "OpenSans-Italic": require("../assets/fonts/OpenSans-Italic.ttf"),
    "OpenSans-Light": require("../assets/fonts/OpenSans-Light.ttf"),
    "OpenSans-Regular": require("../assets/fonts/OpenSans-Regular.ttf"),
    "OpenSans-SemiBold": require("../assets/fonts/OpenSans-SemiBold.ttf"),
  });

  if (isLoaded) {
    SplashScreen.hideAsync();
  }

  const [isDeviceRegistered, setIsDeviceRegistered] = useState(null);

  useEffect(() => {
    const checkDeviceRegistration = async () => {
      const deviceRegistered = await AsyncStorage.getItem("deviceRegistered");
      setIsDeviceRegistered(deviceRegistered === "true");
    };

    checkDeviceRegistration();
  }, []);

  return (
    <FCMProvider>
      <SafeAreaView className="flex-1 justify-center p-4">
        {isDeviceRegistered ? <></> : <InitScreen />}
      </SafeAreaView>
    </FCMProvider>
  );
}
