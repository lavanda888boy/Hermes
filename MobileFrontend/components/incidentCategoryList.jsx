import { View, Text } from "react-native";
import Checkbox from "expo-checkbox";

const IncidentCategoryList = ({ categories, selectedCategories, toggleCheckbox }) => {
  return (
    <View className="flex flex-column justify-center items-center px-4 mt-4">
      {categories.map((category) => (
        <View key={category} className="flex flex-row items-center w-11/12 p-2">
          <Checkbox
            value={selectedCategories.includes(category)}
            onValueChange={() => toggleCheckbox(category)}
          />
          <Text className="text-lg text-primary font-ops-sb ml-4">{category}</Text>
        </View>
      ))}
    </View>
  );
};

export default IncidentCategoryList;