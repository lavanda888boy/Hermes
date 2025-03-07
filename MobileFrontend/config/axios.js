import axios from "axios";

const notificationPreferencesApi = axios.create({
  baseURL: `${process.env.API_GATEWAY_URL}/notification-preferences`,
  headers: {
    "Content-Type": "application/json",
  }
});

export default notificationPreferencesApi;