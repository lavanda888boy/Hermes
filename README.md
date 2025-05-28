<p align="center">
   <img src=./MobileFrontend/assets/images/icon.png width="25%" />
</p>
<h1 align="center">Hermes - Real-Time Notification System</h1>

Microservice-based distributed system with both mobile and web clients for sending real-time notifications about natural disasters and emergencies.

## Introduction

The initial idea was to develop a prototype of a highly available system for sending critical notifications to the users about various incidents which may affect them. It was obvious that the solution is not going to address all the requirements of a similar real-world system therefore certain simplifications were made, having preserved at the same time the main functionality of a real-time notification system even with some enhancements, considering the existing solutions.

* The system gives users the possibility to receive real-time notifications based on their location and notification preferences, and to personalize them by choosing to receive certain types of alerts. It means that there is the list of incident categories about which users should be alerted in any case (floods, wildfires, earthquakes, etc.) and the set of categories which users may subscribe to, for example, Weather and Air Quality.

* The users may report the incident which happened in their area and also view the history of incident notifications they've already received.

* The administrators can observe in real-time user reports, validate or discard them and also report the incidents by themselves according to the verified sources.

All of these features are related to the particular components of the system: their structure and communication is explained in the next section.

## Architecture Overview

The following UML component diagram presented the general structure of this real-time notification system:

The diagram demonstrates the main components of the system: **Remote Backend Server** and **Client Mobile Application**. The **AdminDashboard** is not taken into consideration as its structure is relatively simple and does not require long explanations (its interface can be viewed in the demo section).

The **Remote Backend Server** represents a distributed application based on four major microservices with a gateway in front of them:

* **AdminAuthenticationService** performs basic login/registration operations and issues a JWT token to the admin users to have access to the incident report and validation actions. It is the only service which is not accessible from the gateway but instead should be called directly.
  
* **NotificationPreferencesService** registers user prederences on the incident notifications and interracts with **MongoDB** where it stores incident categories and user preferences mapped to the user device identifiers (Firebase Cloud Messaging (FCM) Tokens).
  
* **GPSLocationTrackingService** processes requests from the user devices and stores their GPS location inside of the **Redis Geo Index** using as key device FCM tokens.
  
* **IncidentRegistrationService** gives admins the possibility to view/report/validate/update/delete incidents and also permits the users to report emergency situation. However, it is important to mention several moments here. First of all, this service validates user reports in order to check whether similar reports were already submitted (by checking the radius of the incident area and its category). Secondly, it performs notification transmission using **Firebase Cloud Messaging** service based in user notification preferences and their current GPS location.

The **Client Mobile Application** makes use of the apis exposed by the backend server and contains the following modules:

* **UI** represents the interface of the application itself visible to the user, its overview is available in the demo section.

* **GPS location tracking background task** is the task which runs from the moment the application is first opened and periodically sends user device GPS location to the backend server. Moreover, the task only sends new data when the previous location has changed by a certain value expressed by the area radius.

* **Notification processing routine** is responsible for handling incoming **Firebase push-notifications**. Upon receiving the notification, it stores the value of the message in the **Temporary storage** which, by the way, is persistent in case of application shutdown. The storage is called temporary because it maintains a certain number of notifications having been received by the device and the list of them from time to time.


## Technology Stack and Communication Patterns

- Server Backend: **.NET Aspire** + **SignalR** + **MongoDB** + **Redis**
- Mobile Client: **Expo React Native** + **Expo Notifications** + **Async Storage** + **NativeWind**
- Admin Web Client: **Angular** + **SignalR** + **Material Design**
- Push notification service: **Firebase Cloud Messaging**

The communication between the client components of the system and the server are mainly performed via **HTTP** requests. The only exception is the real-time connection between the web client and the server which uses **SignalR** with **Websockets** as the transport protocol. This setup can be changed to **SSE** or **Long polling** options. The rest of the commmunication is performed via **Firebase push notifications** (from the server to the mobile client).

## Setup and Execution

#### Server Backend

#### Mobile Client

#### Admin Web Client

1. Navigate to the `AdminDashboard` folder from the root of the repository and run the following commands:
   
   ```bash
   npm install
   ng serve
   ```
   
2. By default the Angular web page will be available at `http://localhost:4200`
