import axios from "axios";

export const getImage = () => {
    return axios.get("http://localhost:5169/api/Image/Upload")
}

export default {
    getImage
}