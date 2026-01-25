import axios from "axios";

export const getTicketById = (id) => {
    return axios.get(`http://localhost:5169/api/Account/${id}`);
}
export default {
    getTicketById,
  };