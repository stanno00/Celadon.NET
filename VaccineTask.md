# Vaccines

Your job is to create a web API in .Net core to help distribute vaccines among hospitals and applicants.

## Entities

#### Vaccine

Every vaccine has a:

- Name
- Price

#### Hospital

Every hospital has a:

- Name
- Budget

#### Applicant

Every applicant has a:

- Name
- Date of birth
- Social security number
  - in nnn-nnn-nnn format
    - n => number
  - it must be unique
- Gender
  - Male or Female or Other

#### Application

Every application has a:

- Applicant name
- Applicant's social security number
- Hospital Name
- Date of the application
- The name of the vaccine the applicant wants
- Date of getting the vaccine

#### Vaccine Order

Every Vaccine Order has a:

- Name of the vaccine
- Name of the hospital
- The number of vaccines being ordered
- The total price of the vaccines

You need to find a way to store the following information:

- Hospital name
- Vaccine name
- Number of vaccines the hospital has

## Features

#### Hospital

- A hospital should be able to order vaccines if they have the budget for it
  - If they don't have the budget return an error message
  - You have to substract the total price of the vaccines from the hospital's budget
- There is an endpoint where a new hospital can be added to the database
- There is an endpoint that updates a hospital's name

#### Applicant

- An applicant should be able to apply for a vaccine in any hospital
  - The application is successful if the choosen hospital has the choosen vaccine in stock. If not return a nice error message.
  - The date of getting the vaccine can not be earlier than the application for the vaccine
  - Every applicant can apply for 3 vaccines in total
  - When the application is succesful remove one vaccine from hospital's stock
- There should be an endpoint for adding new applicants to the database
- There should be an endpoint for removing an applicant by Id

#### Vaccine

- There should be endpoints for:
  - Creating a new vaccine
    - The name of the vaccine is unique
  - Deleting a vaccine
  - Updating the vaccine's name
  - Get a vaccine by Id

## Statistics endpoints

- Return the top 3 most popular vaccine amongst the applicants

  - Vaccine name
  - How many applicants applied for it

```json
{
    "vaccine_name": "Pfizer",
    "num_of_applicants": 3112
}
```

- You get a vaccine name in the url

  - Return all the hospital names that have the vaccine in stock

```json
{
    [
        {
            "hospital_name": "xyz1"
        },
        {
            "hospital_name": "xyz2"
        }
    ]
}
```

- You get a vaccine name in the url

  - Return the most expensive order of the given vaccine

```json
{
  "hospital_name": "xyz",
  "vaccine_name": "Pfizer",
  "num_of_vaccines": 3112,
  "total_price": 9999999
}
```

- You get a vaccine name in the url
  - Return the number of applicants for the vaccine by gender

```json
{
  "vaccine_name": "Pfizer",
  "male": 31122,
  "female": 64423,
  "other": 1231
}
```
