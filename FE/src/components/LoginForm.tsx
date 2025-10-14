import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Login } from "../api/authenticate";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const response = await Login(email, password);
      console.log("Login avvenuto:", response.data);

      localStorage.setItem("token", response.data.token);

      navigate("/table");

    } catch (err) {
      console.error("Errore durante il login:", err);
      setError("Email o password non validi");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleLogin} className="container mt-5" style={{ maxWidth: "400px" }}>
      <h2 className="mb-4 text-center">Accedi</h2>

      <div className="mb-3">
        <label>Email</label>
        <input
          type="email"
          className="form-control"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </div>

      <div className="mb-3">
        <label>Password</label>
        <input
          type="password"
          className="form-control"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>

      {error && <div className="alert alert-danger">{error}</div>}

      <button type="submit" className="btn btn-primary w-100" disabled={loading}>
        {loading ? "Accesso in corso..." : "Accedi"}
      </button>
    </form>
  );
}
