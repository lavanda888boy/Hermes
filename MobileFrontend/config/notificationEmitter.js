import EventEmitter from "eventemitter3";

const notificationEmitter = new EventEmitter();

export const emitNotificationUpdate = (updatedNotifications) => {
  notificationEmitter.emit("notificationsUpdated", updatedNotifications);
};

export const notificationListener = notificationEmitter;