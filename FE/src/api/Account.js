import axios from "axios";

export const getAllAccount = () => {
    return axios.get("http://localhost:5169/api/Account/All")
}

export default {
    getAllAccount,
  };