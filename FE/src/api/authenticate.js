import axios from "axios";

export const Register = (nome, password, cognome, email, dataCreazione, oreLavorate, isAdmin) => {
    return axios.post("http://localhost:5169/api/AuthManagment/register", { nome, password, cognome, email, dataCreazione, oreLavorate, isAdmin });
}

export const Login = (email, password) => {
    return axios.post("http://localhost:5169/api/AuthManagment/login", { email, password });
}