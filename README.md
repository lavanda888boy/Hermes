<p align="center">
   <img src=./MobileFrontend/assets/images/icon.png width="25%" />
</p>
<h1 align="center">Hermes - Real-Time Notification System</h1>

Microservice-based distributed system with both mobile and web clients for sending real-time notifications about natural disasters and various incidents.

## Introduction

The initial idea was to develop a prototype of a highly available system for sending critical notifications to the users about various incidents which may affect them. It was obvious that the solution is not going to address all the requirements of a similar real-world system therefore certain simplifications were made, having preserved at the same time the main functionality of a real-time notification system even with some enhancements, considering the existing solutions.

* The system gives users the possibility to receive real-time notifications based on their location and notification preferences, and to personalize them by choosing to receive certain types of alerts. It means that there is the list of incident categories about which users should be alerted in any case (floods, wildfires, earthquakes, etc.) and the set of categories which users may subscribe to, for example, Weather and Air Quality.

* The users may report the incident which happened in their area and also view the history of incident notifications they've already received.

* The administrators can observe in real-time user reports, validate or discard them and also report the incidents by themselves according to the verified sources.

All of these features are related to the particular components of the system: their structure and communication is explained in the next section.

## Architecture Overview

## Technology Stack and Communication Patterns

- Server Backend: **.NET Aspire** + **SignalR** + **MongoDB** + **Redis**
- Mobile Client: **Expo React Native**
- Admin Web Client: **Angular**
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
