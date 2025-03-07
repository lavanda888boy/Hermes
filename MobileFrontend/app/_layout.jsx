import "../global.css";
import InitScreen from "./index.jsx";
import * as SplashScreen from "expo-splash-screen";
import { useFonts } from "expo-font";
import { SafeAreaView } from "react-native-safe-area-context";
import useFCMToken from "../config/useFCMToken";

SplashScreen.preventAutoHideAsync();

export default function App() {
  const [isLoaded] = useFonts({
    "OpenSans-Italic": require("../assets/fonts/OpenSans-Italic.ttf"),
    "OpenSans-Light": require("../assets/fonts/OpenSans-Light.ttf"),
    "OpenSans-Regular": require("../assets/fonts/OpenSans-Regular.ttf"),
    "OpenSans-SemiBold": require("../assets/fonts/OpenSans-SemiBold.ttf"),
  });

  const fcmToken = useFCMToken();

  if (isLoaded && fcmToken !== "") {
    SplashScreen.hideAsync();
  }

  return (
    <SafeAreaView className="flex-1 justify-center p-4">
      <InitScreen deviceToken={fcmToken} />
    </SafeAreaView>
  );
}
