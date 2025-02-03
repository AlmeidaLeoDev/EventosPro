import {createGlobalStyle} from 'styled-components'

const myGlobalStyles = createGlobalStyle`

/* Basic Reset */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }
  
  body {
    font-family: Arial, sans-serif;
    background-color: #f5f5f5;
    color: #333;
    line-height: 1.6;
  }
  
  /* Centralaized Container */
  .container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
  }
  
  /* Headers */
  h1, h2, h3 {
    margin-bottom: 15px;
  }
  
  /* Button */
  button {
    background-color: #007BFF;
    color: #fff;
    border: none;
    padding: 10px 15px;
    cursor: pointer;
    border-radius: 4px;
  }
  
  button:hover {
    background-color: #0056b3;
  }
  
  /* Form */
  form {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  
  input, select {
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
  }

  .event-form {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin: 20px 0;
  }
  
  .event-form div {
    display: flex;
    flex-direction: column;
  }
  
  .event-form label {
    font-weight: bold;
    margin-bottom: 5px;
  }
  
  .event-form input {
    padding: 8px;
    font-size: 1rem;
  }
  
  button {
    padding: 10px;
    font-size: 1rem;
    cursor: pointer;
  }
  
  .delete-btn {
    background-color: #d9534f;
    color: #fff;
    border: none;
    margin-top: 10px;
  }
  
  `
  export default myGlobalStyles