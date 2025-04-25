import { View } from "react-native";
import { useContext, useEffect, useState } from "react";
import { ApiContext } from "../../config/apiContext";
import IncidentCategoryList from "../../components/incidentCategoryList";
import SubmitButton from "../../components/submitButton";
import { useRouter } from "expo-router";
import notificationPreferencesApi from "../../config/axios";
import SnackBarMessage from "../../components/snackBarMessage";

const Profile = () => {
  const { fcmToken, optionalIncidentCategories, selectedCategories, setSelectedCategories } = useContext(ApiContext);
  const [initialCategories, setInitialCategories] = useState([]);

  const [snackbarVisible, setSnackbarVisible] = useState(false);

  const router = useRouter();

  useEffect(() => {
    if (initialCategories.length === 0 && selectedCategories.length > 0) {
      setInitialCategories([...selectedCategories]);
    }
    router.replace("/init");
  }, [selectedCategories]);

  const toggleCheckbox = (category) => {
    setSelectedCategories((prev) =>
      prev.includes(category)
        ? prev.filter((item) => item !== category)
        : [...prev, category]
    );
  };

  const handleNotificationPreferencesSubmit = async () => {
    try {
      await notificationPreferencesApi.put(`/${fcmToken}`, selectedCategories);
      setInitialCategories([...selectedCategories]);
    } catch (error) {
      console.log(error);

      setSnackbarVisible(true);

      setTimeout(() => {
        setSnackbarVisible(false);
      }, 3000);
    }
  }

  const categoriesHaveChanges =
    initialCategories.length > 0 &&
    JSON.stringify(initialCategories.sort()) !== JSON.stringify(selectedCategories.sort());


  return (
    <View className="flex-1 justify-center p-4 bg-white">
      <IncidentCategoryList
        categories={optionalIncidentCategories}
        selectedCategories={selectedCategories}
        toggleCheckbox={toggleCheckbox}
      />

      <SubmitButton
        title="Save preferences"
        onPress={handleNotificationPreferencesSubmit}
        disabled={!categoriesHaveChanges}
      />

      <SnackBarMessage
        snackbarVisible={snackbarVisible}
        textMessage="⛔ An error occurred. Try again later."
        styleSheet={{
          backgroundColor: "#1976D2",
          position: "absolute",
          bottom: 0,
          left: 15
        }}
      />
    </View >
  );
};

export default Profile;