import { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import useAuth from "../constants/useAuth";

const RegisterForm = () => {
  const { register } = useAuth();

  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isAdmin, setIsAdmin] = useState("");

  const handleSubmitRegister = async (e) => {
    e.preventDefault();
    try {
      await register(email, password);
      alert("Registrazione riuscita ✅");
    } catch {
      alert("Errore registrazione ❌");
    }
  };

  return (
    <>
      <div className="row mb-3">
        <div className="col col-6">
          <div className="card" style={{ width: "18rem", margin: "1rem" }}>
            <div className="card-body">
              <h5 className="card-title">Registrati</h5>
              <p classname="card-text">Esegui la registrazione</p>
              <form onSubmit={handleSubmitRegister} style={{ margin: "1rem" }}>
                <div>
                  <label for="exampleFormControlInput1" className="form-label">
                    Password
                  </label>
                  <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Password"
                    className="form-control"
                  />
                </div>
                <div>
                  <label for="exampleFormControlInput1" className="form-label">
                    Email
                  </label>
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Email"
                    className="form-control"
                  />
                </div>
                <div>
                  <label for="exampleFormControlInput1" className="form-label">
                    isAdmin
                  </label>
                  <input
                    type="checkbox"
                    value={isAdmin}
                    onChange={(e) => setIsAdmin(e.target.value)}
                    placeholder="Email"
                    className="checkbox"
                  />
                </div>
                <button type="submit" className="btn btn-success">
                  Register
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default RegisterForm;
