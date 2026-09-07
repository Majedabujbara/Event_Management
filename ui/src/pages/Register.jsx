import { useState, useRef, useEffect } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import Input2 from "../components/Input2";
import { Button, Snackbar, Alert } from "@mui/material";

export default function Register() {
  const [personName, setPersonName] = useState("");
  const [phone, setPhone] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmationPassword, setConfirmationPassword] = useState("");

  const [passwordEmpty, setPasswordEmpty] = useState(false);
  const [passwordNotEqual, setPasswordNotEqual] = useState(false);
  const [emailNotValid, setEmailNotValid] = useState(false);
  const [nameEmpty, setNameEmpty] = useState(false);
  const [numberEmpty, setNumberEmpty] = useState(false);
  const [error, setError] = useState(false);
  const [registerError, setRegisterError] = useState();

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const containerRef = useRef(null);
  const [hasAnimated, setHasAnimated] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const triggerAnimation = () => {
    if (!hasAnimated && containerRef.current) {
      containerRef.current.classList.add("animated-once");
      setHasAnimated(true);
    }
  };

  useEffect(() => {
    triggerAnimation();
  }, [location]);

  function handleMouseEnter() {
    triggerAnimation();
  }

  async function handleSubmit(event) {
    event.preventDefault();
    let hasError = false;

    const fd = new FormData(event.target);
    const data = Object.fromEntries(fd.entries());

    if (!data.email.includes("@")) {
      setEmailNotValid(true);
      hasError = true;
    } else {
      setEmailNotValid(false);
    }

    if (!data.password.trim()) {
      setPasswordEmpty(true);
      hasError = true;
    } else {
      setPasswordEmpty(false);
    }

    if (!data.personName.trim()) {
      setNameEmpty(true);
      hasError = true;
    } else {
      setNameEmpty(false);
    }

    if (!data.phone.trim()) {
      setNumberEmpty(true);
      hasError = true;
    } else {
      setNumberEmpty(false);
    }

    if (data.password !== data.confirmationPassword) {
      setPasswordNotEqual(true);
      hasError = true;
    } else {
      setPasswordNotEqual(false);
    }

    if (hasError) return;

    try {
      const response = await fetch(
        "https://localhost:7262/api/Account/Register",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(data),
        }
      );

      const result = await response.json();

      if (response.ok) {
        setSnackbarOpen(true);
        localStorage.setItem("regGood", "true");
        navigate("/login");
      } else {
        setError(true);
        setRegisterError(
          result.detail || result.errors?.Phone?.[0] || "Registration failed"
        );
      }
    } catch (error) {
      console.error("Error registering user:", error);
    }
  }

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  return (
    <div
      className="container"
      ref={containerRef}
      onMouseEnter={handleMouseEnter}
    >
      <div className="top"></div>
      <div className="bottom"></div>
      <form className="center" onSubmit={handleSubmit}>
        <h2>Registration</h2>

        <div className="input-wrapper">
          <Input2
            label="Name"
            name="personName"
            value={personName}
            onChange={(e) => setPersonName(e.target.value)}
            error={nameEmpty}
            errorText="Please enter a name"
          />
        </div>

        <div className="input-wrapper">
          <Input2
            label="Phone"
            name="phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
            error={numberEmpty}
            errorText="Please enter a valid number"
          />
        </div>

        <div className="input-wrapper">
          <Input2
            label="Email"
            name="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            error={emailNotValid}
            errorText="Please enter a valid email address"
          />
        </div>

        <div className="input-wrapper">
          <Input2
            label="Password"
            type="password"
            name="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            error={passwordEmpty}
            errorText="Please enter a password"
          />
        </div>

        <div className="input-wrapper">
          <Input2
            label="Confirm Password"
            type="password"
            name="confirmationPassword"
            value={confirmationPassword}
            onChange={(e) => setConfirmationPassword(e.target.value)}
            error={passwordNotEqual}
            errorText="Passwords do not match"
          />
        </div>

        {registerError && <p className="err">{registerError}</p>}

        <Button
          variant="contained"
          color="success"
          type="submit"
          sx={{ marginTop: "20px" }}
        >
          Register
        </Button>

        <div className="social-links">
          <a
            href="https://www.facebook.com/share/1CMLv3WQdh/?mibextid=qi2Omg"
            target="_blank"
            rel="noopener noreferrer"
          >
            Facebook
          </a>
          <a
            href="https://www.bau.edu.jo/"
            target="_blank"
            rel="noopener noreferrer"
          >
            <img
              src="/Images/BAUClubs.png"
              alt="BAU Clubs"
              className="bau-image"
            />
          </a>
          <a
            href="https://www.linkedin.com/school/albalqa-applied-university/"
            target="_blank"
            rel="noopener noreferrer"
          >
            LinkedIn
          </a>
          
        </div>

        <p className="no-acc">
          Already have an account? <Link to="/login">Login here</Link>
        </p>
      </form>

      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity="success"
          variant="filled"
        >
          Registration successful!
        </Alert>
      </Snackbar>
    </div>
  );
}
