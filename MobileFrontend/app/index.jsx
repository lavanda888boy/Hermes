import { SafeAreaView, View, Text, Image } from "react-native";
import { useContext } from "react";
import notificationPreferencesApi from "../config/axios";
import { ApiContext } from "../config/apiContext";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useRouter } from "expo-router";
import IncidentCategoryList from "../components/incidentCategoryList";
import SubmitButton from "../components/submitButton";

const InitScreen = () => {
  const { fcmToken, incidentCategories, selectedCategories, setSelectedCategories } = useContext(ApiContext);
  const router = useRouter();

  const toggleCheckbox = (category) => {
    setSelectedCategories((prev) =>
      prev.includes(category)
        ? prev.filter((item) => item !== category)
        : [...prev, category]
    );
  };

  const handleNotificationPreferencesSubmit = async () => {
    try {
      if (selectedCategories.length > 0) {
        await notificationPreferencesApi.post(`/${fcmToken}`, selectedCategories);
      }

      await AsyncStorage.setItem("deviceRegistered", "true");
      router.replace("/(tabs)/home");
    } catch (error) {
      console.log(error);
    }
  }

  return (
    <SafeAreaView className="flex-1 justify-center p-4">
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
          (earthquakes, floods, wildfires, etc.). If you wish you can also subscribe to some
          additional notifications from the list below. Your preferences can be
          later change.
        </Text>

        <IncidentCategoryList
          categories={incidentCategories}
          selectedCategories={selectedCategories}
          toggleCheckbox={toggleCheckbox}
        />

        <SubmitButton
          onPress={handleNotificationPreferencesSubmit}
          title="Continue"
        />
      </View>
    </SafeAreaView>
  );
};

export default InitScreen;
