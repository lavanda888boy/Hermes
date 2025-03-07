import { createContext, useState, useEffect } from "react";
import * as Notifications from "expo-notifications";

export const FCMContext = createContext();

export const FCMProvider = ({ children }) => {
  const [fcmToken, setFcmToken] = useState("");

  useEffect(() => {
    const getFcmToken = async () => {
      const { status } = await Notifications.requestPermissionsAsync();

      if (status !== "granted") {
        alert("You need to enable permissions in order to receive notifications.");
        return;
      }

      try {
        const tokenResponse = await Notifications.getDevicePushTokenAsync();
        setFcmToken(tokenResponse.data);
      } catch (error) {
        console.log(error);
      }
    };

    getFcmToken();
  }, []);

  return (
    <FCMContext.Provider value={{ fcmToken }}>
      {children}
    </FCMContext.Provider>
  );
};
