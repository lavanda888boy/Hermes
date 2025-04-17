import { View } from "react-native";
import { useContext, useEffect, useState } from "react";
import { ApiContext } from "../../config/apiContext";
import IncidentCategoryList from "../../components/incidentCategoryList";
import SubmitButton from "../../components/submitButton";
import { useRouter } from "expo-router";
import notificationPreferencesApi from "../../config/axios";

const Profile = () => {
  const { fcmToken, incidentCategories, selectedCategories, setSelectedCategories } = useContext(ApiContext);
  const [initialCategories, setInitialCategories] = useState([]);

  const router = useRouter();

  useEffect(() => {
    if (initialCategories.length === 0 && selectedCategories.length > 0) {
      setInitialCategories([...selectedCategories]);
    }
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
      router.replace("/home");
    } catch (error) {
      console.log(error);
    }
  }

  const categoriesHaveChanges =
    initialCategories.length > 0 &&
    JSON.stringify(initialCategories.sort()) !== JSON.stringify(selectedCategories.sort());


  return (
    <View className="flex-1 justify-center p-4 bg-white">
      <IncidentCategoryList
        categories={incidentCategories}
        selectedCategories={selectedCategories}
        toggleCheckbox={toggleCheckbox}
      />

      <SubmitButton
        title="Save preferences"
        onPress={handleNotificationPreferencesSubmit}
        disabled={!categoriesHaveChanges}
      />
    </View>
  );
};

export default Profile;