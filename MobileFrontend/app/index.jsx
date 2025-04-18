import { ActivityIndicator, View } from "react-native";

const LoadScreen = () => {
  return (
    <View className="flex-1 items-center justify-center bg-white">
      <ActivityIndicator size="large" color="#1976D2" />
    </View>
  );
}

export default LoadScreen;