import { createContext, useState, useEffect } from "react";
import * as Notifications from "expo-notifications";
import { notificationPreferencesApi } from "./axios";

export const ApiContext = createContext();

export const ApiContextProvider = ({ children }) => {
  const [fcmToken, setFcmToken] = useState("");
  const [incidentCategories, setIncidentCategories] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);

  useEffect(() => {
    const getFcmToken = async () => {
      try {
        const { status } = await Notifications.requestPermissionsAsync();

        if (status !== "granted") {
          alert("You need to enable notification permissions to use the app.");
          return;
        }

        const tokenResponse = await Notifications.getDevicePushTokenAsync();
        setFcmToken(tokenResponse.data);
        console.log("FCM Token:", tokenResponse.data);
      } catch (error) {
        console.error("Error getting FCM token:", error);
      }
    };

    getFcmToken();
  }, []);

  useEffect(() => {
    if (fcmToken === "") return;

    const fetchApiData = async () => {
      try {
        const incidentCategoriesResponse = await notificationPreferencesApi.get("/");
        setIncidentCategories(incidentCategoriesResponse.data);

        const selectedCategoriesResponse = await notificationPreferencesApi.get(`/${fcmToken}`);
        setSelectedCategories(selectedCategoriesResponse.data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchApiData();
  }, [fcmToken]);

  return (
    <ApiContext.Provider
      value={{ fcmToken, incidentCategories, selectedCategories, setSelectedCategories }}
    >
      {children}
    </ApiContext.Provider>
  );
};
