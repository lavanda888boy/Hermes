import { createContext, useState, useEffect } from "react";
import * as Notifications from "expo-notifications";
import { notificationPreferencesApi } from "./axios";
import AsyncStorage from "@react-native-async-storage/async-storage";

export const ApiContext = createContext();

export const ApiContextProvider = ({ children }) => {
  const [fcmToken, setFcmToken] = useState("");
  const [allIncidentCategories, setAllIncidentCategories] = useState([]);
  const [optionalIncidentCategories, setOptionalIncidentCategories] = useState([]);
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

        await AsyncStorage.setItem("fcmToken", tokenResponse.data);
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
        const allIncidentCategoriesResponse = await notificationPreferencesApi.get("/");
        setAllIncidentCategories(allIncidentCategoriesResponse.data);

        const optionalIncidentCategoriesResponse = await notificationPreferencesApi.get("/optional");
        setOptionalIncidentCategories(optionalIncidentCategoriesResponse.data);
      } catch (error) {
        console.error(error);
      }

      try {
        const selectedCategoriesResponse = await notificationPreferencesApi.get(`/${fcmToken}`);
        setSelectedCategories(selectedCategoriesResponse.data);
      } catch (error) { }
    };

    fetchApiData();
  }, [fcmToken]);

  return (
    <ApiContext.Provider
      value={{ fcmToken, allIncidentCategories, optionalIncidentCategories, selectedCategories, setSelectedCategories }}
    >
      {children}
    </ApiContext.Provider>
  );
};
