import React from "react";
import { Navigate } from "react-router-dom";
import useAuth from "../constants/useAuth";

const PrivateRouteTable = ({ children }) => {
  const { token } = useAuth();
  return token ? children : <Navigate to="/table" replace />;
};

export default PrivateRouteTable;
