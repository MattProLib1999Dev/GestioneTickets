import { useNavigate } from "react-router-dom";

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    navigate("/table"); // reindirizza alla login page
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light" style={{backgroundImage: '../assets/img/header_conlogo.png'}}>
      <div className="container-fluid">
        <a className="navbar-brand" href="#">Gestione Ticket</a>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <a className="nav-link" href="/table">Home</a>
            </li>
          </ul>
          <button className="btn btn-outline-danger" onClick={handleLogout}>Logout</button>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
