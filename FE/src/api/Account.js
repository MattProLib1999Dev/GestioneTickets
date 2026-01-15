import axios from "axios";

export const getAllAccount = () => {
    return axios.get("http://localhost:5169/api/Account/All")
}

export const getAccountById = (id) => {
    return axios.get(`http://localhost:5169/api/Account/${id}`);
}

export default {
    getAllAccount,
    getAccountById,
  };