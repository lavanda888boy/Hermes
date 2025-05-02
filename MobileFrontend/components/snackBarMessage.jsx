import { Snackbar } from "react-native-paper";
import { Text } from "react-native";

const SnackBarMessage = ({ snackbarVisible, textMessage, styleSheet }) => {
  return (
    <>
      <Snackbar
        visible={snackbarVisible}
        style={styleSheet}
      >
        <Text className="text-lg color-white font-ops-r">
          {textMessage}
        </Text>
      </Snackbar>
    </>
  )
}

export default SnackBarMessage;