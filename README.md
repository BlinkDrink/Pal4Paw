# Pal4Paw

---

## Softuni-ASP.NET Core Web Application

### Pal4Paw is an online dogcare platform. It's purpose is to connect dog owners with dogsitters who are looking for part-time job.

>  Initially both sides should create account(both dogsitters and owners). Owners can register freely, but dogsitters require to be examined first. Dogsitters can candidate by signing in with temporary information like email, password and phone number. Aditionally dogsitters should answer 5 control questions(these are required).

>  Afterwards **the Admin** can approve or reject dogsitters(Admin can log into their profile from both Owner login and Dogsitter login). Once accepted the dogsitter is officially a member of the platform and can interact with owners. Dogsitters should firstly fill their personal information in order to be able to be seen by owners.

>  Once both sides have completely set up their profiles then the owners can send appointment requests to dogsitters chosen by them. Therefore dogsitters receive a notification containing the information about the Owner's request(date of appointment, start time, end time, etc.) and can chose to accept or reject an appointment. Once accepted the appointment is officially saved and can be seen in the **appointment** tab of both sides. Only the dogsitter can initiate an appointment by clicking on the appointment that he/she desires to start(note that an appointment cannot be started unless the day of the appointment matches the current day). After an appointment has been started it tracks time and again only dogsitters can cancel it. Once ended the appointment is counted as Processed and is no more relevants so it's greyed out. In the end both sides should leave feedback to the other person. Owners can access the feedback input page by clicking on the notification after end of appointment. Dogsitters on the other hand are directly redirected to leave feedback after they end an appointment.

## The work flow is as follows

Action | Owner | Dogsitter
--- | --- | ---
*Profile* | `Create profile` | **Create profile**
*Find*  | Find dogsitter | Successfully pass examination
*Browse*  | Browse, chose and pick a dogsitter | Fill your profile
*Time*  | Enjoy your free time | Care for someone's dog
*Pay*  | Pay the dogsitter | Get payment
*Again*  | Use Pal4Paw again | Get new appointment
