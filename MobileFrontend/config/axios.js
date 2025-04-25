import axios from "axios";

const notificationPreferencesApi = axios.create({
  baseURL: `${process.env.API_GATEWAY_URL}/notification-preferences`,
  headers: {
    "Content-Type": "application/json",
  }
});

const gpsTrackingApi = axios.create({
  baseURL: `${process.env.API_GATEWAY_URL}/gps-tracking`,
  headers: {
    "Content-Type": "application/json",
  }
});

const incidentRegistrationApi = axios.create({
  baseURL: `${process.env.API_GATEWAY_URL}/incident-registration`,
  headers: {
    "Content-Type": "application/json",
  }
});

export { notificationPreferencesApi, gpsTrackingApi, incidentRegistrationApi };