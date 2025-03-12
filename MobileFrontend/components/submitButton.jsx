import { TouchableOpacity, Text } from "react-native";

const SubmitButton = ({ onPress, title }) => {
  return (
    <TouchableOpacity
      className="bg-primary rounded-3xl py-3 mt-10 mx-12"
      activeOpacity={0.7}
      onPress={onPress}
    >
      <Text className="text-xl text-center text-white font-ops-sb">
        {title}
      </Text>
    </TouchableOpacity>
  );
};

export default SubmitButton;
