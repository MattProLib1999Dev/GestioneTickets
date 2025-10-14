import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Navbar from "./components/Navbar";
import LoginForm from "./components/LoginForm";
import RegisterForm from "./components/RegisterForm";
import Table from "./components/Table";

const App = () => {
  const handleLogin = (formData) => {
    console.log("Login form submitted:", formData);
    // Add your login logic here
  };
  return (
    <Router>
      <Navbar />
      <Routes>
        {/* Pagina di login */}
        <Route path="/" element={<LoginForm onSubmit={handleLogin} />} />

        {/* Registrazione */}
        <Route path="/register" element={<RegisterForm />} />

        {/* Pagina protetta */}
        <Route
          path="/table"
          element={
              <Table />
          }
        />

        {/* Catch-all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
};

export default App;
