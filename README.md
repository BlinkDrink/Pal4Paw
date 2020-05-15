# Pal4Paw

## Softuni- ASP.NET Core Web Application

### Pal4Paw is an online dogcare platform. It's purpose is to connect dog owners with dogsitters who are looking for part-time job.

## The work flow is as follows

Action | Owner | Dogsitter
--- | --- | ---
Profile	 | Create profile | Create profile
Find | Find dogsitter | Successfully pass examination
Browse | Browse, chose and pick a dogsitter | Fill your profile
Time | Enjoy your free time | Care for someone's dog
Pay | Pay the dogsitter | Get payment
Repeat | Use Pal4Paw again | Get new appointment

## How to use Pal4Paw
Initially both sides should create account(both dogsitters and owners). Owners can register freely, but dogsitters require to be examined first. Dogsitters can candidate by signing in with temporary information like email, password and phone number. Aditionally dogsitters should answer 5 control questions(these are required).

### 1. Account creation
Pal4Paw connects two types of users <i class="icon-user"></i> -- Dogsitters and Owners.

Both sides should create an account before accessing the functionality of the website.

Owner | Dogsitter
--- | ---
Owners can [create an account](https://res.cloudinary.com/dnonuly2u/image/upload/v1589561126/CreateAccOwner_vck3lp.png) right off the bat <i class='icon-ok-circled'></i>. Once successfully registered, the user can use the website and browse for dogsitters who are looking for part--time job.| Dogsitters on the other hand cannot <i class='icon-cancel-circled'> create an account instantly. They should send an [application](https://res.cloudinary.com/dnonuly2u/image/upload/v1589561049/Candidate_ifv9dr.png) to be part of the platform since they are required to have certain traits in order to become a dogsitter on our platform. 

> **Note**: A dogsitter should be approved by a third party (in this case the admin user)

 takes care of approval <i class='icon-thumbs-up-alt'> or rejection <i class=' icon-thumbs-down-alt'> of applicants)  

Once approved by the admin the user is officially a **dogsitter** and can access their profile<i class=' icon-ok'>.

> **Note**: A dogsitter should fill their profile info <i class='icon-user'> in order to be seen by the owners who are browsing for dogsitters.

### 2. Finding dogsitter and sending appointment request
Once both sides have completely set up their profiles then the owners can send appointment requests <i class=' icon-direction'> to dogsitters chosen by them. 

#### How does that work? **Pretty simple!**

Owners have a [tab](https://res.cloudinary.com/dnonuly2u/image/upload/v1589562341/FindDogsitter_ewnh0q.png) on their navigation bar where they can access the list of available dogsitters<i class='icon-check'>! When opened, the page will contain a list of dogsitters which the owner can choose of.

![enter image description here](https://res.cloudinary.com/dnonuly2u/image/upload/v1589562630/FindDogsitterList_kuxlut.png)

> This page shows all of the dogsitters and short information about them such as Name, Description, Wage rate and Star rating <i class='icon-star'>

When the owner finds a dogsitter who has picked their interest the owner can [click](https://res.cloudinary.com/dnonuly2u/image/upload/v1589563128/DetailsDogsitter_kb5ulo.png) on that dogsitter card and see detailed information about the dogsitter. In addition there is a button to send [appointment request](https://res.cloudinary.com/dnonuly2u/image/upload/v1589563173/AppointmentRequest_qqw0dg.png) with given date, start hour and end hour.

---

- Once the appointment request has been sent the dogsitter [receives a notification](https://res.cloudinary.com/dnonuly2u/image/upload/v1589563363/NotificationsPanelDogsitterAppointmnet_hs2pda.png) <i class='icon-bell-alt'> containing the [information](https://res.cloudinary.com/dnonuly2u/image/upload/v1589563399/DetailedInfoAboutAppointment_zp7bq5.png) about the Owner's request(date of appointment, start time, end time, etc.) and can chose to accept or reject an appointment.

- Once accepted the appointment is officially saved and can be seen in the **appointment** tab of both sides. Only the dogsitter can initiate an appointment by [clicking on the appointment that he/she desires to start](https://res.cloudinary.com/dnonuly2u/image/upload/v1589563871/StartAppointment_dqzxvs.png)
><i class='icon-attention'> **Note**: an appointment cannot be started unless the day of the appointment matches the current day 

- After an [appointment has been started](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564085/StartedAppointment_ssjxe4.png) it tracks time and again only dogsitters can cancel it. When [ended](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564130/Ended_bxexht.png) the appointment is counted as Processed and is no more relevants so it gets greyed out.

- In the end both sides should leave feedback to the other person. Owners can access the [feedback](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564361/OwnerFeedbackLeave_sogtdn.png) input page by clicking on the [notification](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564254/FeedbackOwner_qxqj4e.png) after end of appointment. Dogsitters on the other hand are directly redirected to leave [feedback after they end an appointment.](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564254/FeedbackOwner_qxqj4e.png)

	- The feedback is displayed as a 'comments' <i class='icon-comment'>  section on each user's profile. Dogsitters have their own [comments](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564613/DogsitterComments_jxqky9.png) section where everyone can see what other owners think of that dogsitter and what is their star rating <i class='icon-star'>.
	-  Owners also have their own comments section where they can see their [comments and star rating](https://res.cloudinary.com/dnonuly2u/image/upload/v1589564644/OwnerComments_s8wi1q.png).

## Future work

Right now I am working on this idea and developing it further. It is a good idea for such a community of owners and dogsitters in my country.
