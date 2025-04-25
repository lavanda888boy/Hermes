import { SafeAreaView, View, Text, Image } from "react-native";
import { useContext, useState } from "react";
import notificationPreferencesApi from "../../config/axios";
import { ApiContext } from "../../config/apiContext";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useRouter } from "expo-router";
import IncidentCategoryList from "../../components/incidentCategoryList";
import SubmitButton from "../../components/submitButton";
import SnackBarMessage from "../../components/snackBarMessage";

const InitScreen = () => {
  const { fcmToken, optionalIncidentCategories, selectedCategories, setSelectedCategories } = useContext(ApiContext);
  const [snackbarVisible, setSnackbarVisible] = useState(false);

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
        await AsyncStorage.setItem("deviceRegistered", "true");
        router.replace("/home");
      }
    } catch (error) {
      console.error(error);

      setSnackbarVisible(true);

      setTimeout(() => {
        setSnackbarVisible(false);
      }, 3000);
    }
  }

  return (
    <SafeAreaView className="flex-1 justify-center p-4 bg-white">
      <View className="flex-1 mt-7">
        <Image
          className="w-40 h-40 self-center mb-4"
          source={require("../../assets/images/icon.png")}
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
          categories={optionalIncidentCategories}
          selectedCategories={selectedCategories}
          toggleCheckbox={toggleCheckbox}
        />

        <SubmitButton
          title="Continue"
          onPress={handleNotificationPreferencesSubmit}
          disabled={selectedCategories.length === 0}
        />

        <SnackBarMessage
          snackbarVisible={snackbarVisible}
          textMessage="⛔ An error occurred. Try again later"
          styleSheet={{
            backgroundColor: "#1976D2",
          }}
        />
      </View>
    </SafeAreaView>
  );
};

export default InitScreen;
