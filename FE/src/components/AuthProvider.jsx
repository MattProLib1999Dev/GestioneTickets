import { useState, useEffect, createContext } from "react";
import api from "../api/authenticate";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(() => localStorage.getItem("token"));

  const register = async (email, password) => {
    const response = await api.post("/api/AuthManagment/register", { email, password });
    const newToken = response.data.accessToken;
    setToken(newToken);
    localStorage.setItem("token", newToken);
  };

  const login = async (email, password) => {
    const response = await api.post("/api/AuthManagment/login", { email, password });
    const newToken = response.data.accessToken;
    setToken(newToken);
    localStorage.setItem("token", newToken);
  };

  const logout = () => {
    setToken(null);
    localStorage.removeItem("token");
  };

  // Interceptor una sola volta
  useEffect(() => {
    const authInterceptor = api.interceptors.request.use((config) => {
      if (token) config.headers.Authorization = `Bearer ${token}`;
      return config;
    });

    return () => {
      api.interceptors.request.eject(authInterceptor);
    };
  }, [token]);

  return (
    <AuthContext.Provider value={{ token, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthProvider, AuthContext };
