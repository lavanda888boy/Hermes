import { View, Text, TouchableOpacity, Image } from "react-native";
import React from "react";
import Checkbox from "expo-checkbox";
import { useState, useEffect } from "react";
import notificationPreferencesApi from "../config/axios";

const InitScreen = ({ deviceToken }) => {
  const [incidentCategories, setIncidentCategories] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);

  useEffect(() => {
    const fetchIncidentCategories = async () => {
      try {
        const response = await notificationPreferencesApi.get("/");
        setIncidentCategories(response.data);
      } catch (error) {
        console.log(error);
      }
    };

    fetchIncidentCategories();
  }, []);

  const toggleCheckbox = (category) => {
    setSelectedCategories((prev) =>
      prev.includes(category)
        ? prev.filter((item) => item !== category)
        : [...prev, category]
    );
  };

  const handleNotificationPreferencesSubmit = async () => {
    try {
      await notificationPreferencesApi.post(`/${deviceToken}`, selectedCategories);
    } catch (error) {
      console.log(error);
    }
  }

  return (
    <View className="flex-1 mt-7">
      <Image
        className="w-40 h-40 self-center mb-4"
        source={require("../assets/images/icon.jpg")}
      />

      <Text
        className="text-5xl text-center pt-2 mt-2 text-primary font-ops-sb">
        Welcome to Hermes!
      </Text>

      <Text className="text-lg text-primary text-justify font-ops-r px-4 mt-4">
        You're going to be automatically subscribed to critical incident notifications
        (earthquakes, flood, wildfires, etc.). If you wish you can also subscribe to some
        additional notifications from the list below. Your preferences can be
        later change.
      </Text>

      <View className="flex flex-column justify-center items-center px-4 mt-4">
        {incidentCategories.map((category) => (
          <View key={category} className="flex flex-row items-center w-11/12 p-2">
            <Checkbox
              value={selectedCategories.includes(category)}
              onValueChange={() => toggleCheckbox(category)}
            />
            <Text className="text-lg text-primary font-ops-sb ml-4">{category}</Text>
          </View>
        ))}
      </View >

      <TouchableOpacity
        className="bg-primary rounded-3xl py-3 mt-10 mx-12"
        activeOpacity={0.7}
        onPress={handleNotificationPreferencesSubmit}
      >
        <Text className="text-xl text-center text-white font-ops-sb">
          Continue
        </Text>
      </TouchableOpacity>
    </View>
  );
};

export default InitScreen;
