<a id="readme-top"></a>

<div> 
  <a href="https://www.youtube.com/@OazizDigital/videos" target="_blank"><img src="https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white" target="_blank"></a>
  <a href = "mailto:almeidaleo.dev@gmail.com"><img src="https://img.shields.io/badge/-Gmail-%23333?style=for-the-badge&logo=gmail&logoColor=white" target="_blank"></a>
  <a href="https://www.linkedin.com/in/almeidaleo-dev/" target="_blank"><img src="https://img.shields.io/badge/-LinkedIn-%230077B5?style=for-the-badge&logo=linkedin&logoColor=white" target="_blank"></a> 
</div>



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/AlmeidaLeoDev/EventosPro">
    <img src="assets/screenshots/logo.png" alt="Logo" width="150" height="150">
  </a>

  <h3 align="center">EVENTOS PRO</h3>

  <p align="center">
    Web-based event calendar system
    <br />
    <a href="https://github.com/AlmeidaLeoDev/EventosPro"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="">View Demo</a>
    &middot;
    <a href="https://github.com/AlmeidaLeoDev/EventosPro/issues/new?labels=bug&template=bug-report.md">Report Bug</a>
    &middot;
    <a href="https://github.com/AlmeidaLeoDev/EventosPro/issues/new?labels=bug&template=feature-request.md">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

Event management platform with secure authentication and an integrated invitation system.

Main Objectives:

* Provide an interactive calendar for event management
* Implement a secure JWT authentication system with email confirmation
* Allow the creation, reading, updating, and deletion of events while keeping the data always up-to-date.

</br>

### Built With

**Front-End**

* [![Static Badge](https://img.shields.io/badge/javascript-black?style=for-the-badge&logo=javascript)](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
* [![Static Badge](https://img.shields.io/badge/React-%2323272F?style=for-the-badge&logo=react)](https://react.dev/)
* [![Static Badge](https://img.shields.io/badge/html-white?style=for-the-badge&logo=html5)](https://developer.mozilla.org/en-US/docs/Web/HTML)
* [![Static Badge](https://img.shields.io/badge/css-white?style=for-the-badge&logo=css3&logoColor=%231572B6)](https://developer.mozilla.org/en-US/docs/Web/CSS)

**Back-End**

* [![Static Badge](https://img.shields.io/badge/ASP.NET%20Core-%23512BD4?style=for-the-badge&logo=back-end)](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-9.0)
* [![Static Badge](https://img.shields.io/badge/C%23-%23512BD4?style=for-the-badge)](https://learn.microsoft.com/en-us/dotnet/csharp/)
* [![Static Badge](https://img.shields.io/badge/Entity%20Framework%20Core-%23512BD4?style=for-the-badge&logo=back-end)](https://learn.microsoft.com/en-us/ef/)

**Infrastructure**

* [![Static Badge](https://img.shields.io/badge/microsoft%20sql%20server-white?style=for-the-badge)](https://learn.microsoft.com/en-us/sql/sql-server/?view=sql-server-ver16)

<br />
<p align="left">(<a href="#readme-top">Back to top</a>)</p>
<br />



<!-- GETTING STARTED -->
## Getting Started

Instructions on how you can set up your project locally.

### Prerequisites

* Git: To clone the repository.
* Node.js and npm/yarn: Required for front-end development.
* .NET SDK: To build and run the back-end (ASP.NET Core).
* Database: A running instance of your database (Microsoft SQL Server).
* Email Service Credentials: SMTP settings for email confirmation and notifications.
* MailKit: For handling email functionalities such as sending confirmation emails and notifications.
* FullCalendar: JavaScript library for implementing an interactive calendar interface for event management.


### Installation

Below you will find instructions on how to install and configure your application.

**1. Clone the Repository**
Open your terminal and run:
```bash
git clone https://github.com/AlmeidaLeoDev/EventosPro
```
Then navigate to the project directory:
```bash
cd EventosPro
```
<p></p>

**2. Configure the Back-End**
Set Up Environment Variables:
Create a file (e.g., .env or use your preferred method for environment configuration) in the back-end directory with the following variables:
- JWT_SECRET=your_jwt_secret_key
- DB_CONNECTION_STRING=your_database_connection_string
- SMTP_HOST=your_smtp_host
- SMTP_PORT=your_smtp_port
- SMTP_USER=your_smtp_username
- SMTP_PASS=your_smtp_password
<p></p>

**3. Build and Run the Back-End:**
Navigate to the back-end folder (if separate) and run:
```bash
dotnet build
```
```bash
dotnet run
```
<p></p>

**4. Configure the Front-End**
Navigate to the front-end directory (typically named client or similar):
```bash
cd client
npm install
```
Then, install FullCalendar packages:
```bash
npm install @fullcalendar/core @fullcalendar/daygrid
```
Integrate FullCalendar in your application by importing and configuring it in your React (or other JavaScript framework) component.
Run the Front-End:
Start the development server:
```bash
npm start
```
<p></p>

**5. Verify the Setup**
* Interactive Calendar: Open the application and confirm that the interactive calendar loads correctly.
* Authentication: Test the JWT-based authentication system by signing up and confirming the account via email.

<br />
<p align="left">(<a href="#readme-top">Back to top</a>)</p>
<br />



<!-- USAGE -->
## Usage

Demonstration of how the project can be used

<div align="left">

**1. Registration and Authentication**
  
* The user accesses the homepage and clicks on "Sign up."

<img src="assets/screenshots/register.png" alt="Logo" width="800" height="auto">

* They fill in their details (name, email, password) and submit the form.
* A confirmation email is sent (via MailKit).
* The user clicks the confirmation link to activate their account.
* Now, they can log in using their credentials (JWT Authentication).
<p></p>

**2. If you already have an account, you can click on "Recover password."**

* In this case, the instructions for changing your password will be sent to your email.

<img src="assets/screenshots/forgot-password.png" alt="Logo" width="800" height="auto">
<p></p>

**3. Creating an Event**

* After logging in, the user accesses the dashboard

<img src="assets/screenshots/home.png" alt="Logo" width="800" height="auto">

* In the dashborad the user clicks on "Create Event".
* They fill in details such as date and description.
* The event is saved in the database and displayed in FullCalendar.

<img src="assets/screenshots/create-event.png" alt="Logo" width="800" height="auto">
<p></p>

**4. Managing Events**

* The user can edit event details or delete unwanted events.

<img src="assets/screenshots/edit-event.png" alt="Logo" width="800" height="auto">
<img src="assets/screenshots/edit-eventPage.png" alt="Logo" width="800" height="auto">

</div>

<br />
<p align="left">(<a href="#readme-top">Back to top</a>)</p>
<br />



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Top contributors:

<table>
  <tr>
    <td align="center">
      <a href="#">
        <img src="assets/profile/perfilLinkedin.jpeg" width="100px;" alt="Leonardo Almeida Profile Picture"/><br>
        <sub>
          <b>Leonardo Almeida</b>
        </sub>
      </a>
    </td>
</table>

<br />
<p align="left">(<a href="#readme-top">Back to top</a>)</p>
<br />
