import { TouchableOpacity, Text } from "react-native";

const SubmitButton = ({ title, onPress, disabled }) => {
  return (
    <TouchableOpacity
      className={`rounded-3xl py-3 mt-10 mx-12 ${disabled ? "bg-gray-400" : "bg-primary"}`}
      activeOpacity={disabled ? 1 : 0.7}
      onPress={disabled ? null : onPress}
    >
      <Text className="text-xl text-center text-white font-ops-sb">
        {title}
      </Text>
    </TouchableOpacity>
  );
};

export default SubmitButton;
