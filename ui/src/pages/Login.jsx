import { useState, useRef, useEffect } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import Input2 from "../components/Input2";
import { Alert, Button, Snackbar } from "@mui/material";

export default function Login() {
  const [passwordEmpty, setPasswordEmpty] = useState(false);
  const [emailNotValid, setEmailNotValid] = useState(false);
  const [loginFailed, setLoginFailed] = useState("");
  const [loginError, setLoginError] = useState(false);
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

  useEffect(() => {
    if (localStorage.getItem("regGood")) {
      setSnackbarOpen(true);
      localStorage.removeItem("regGood");
    }
  }, []);

  function handleCloseSnackbar() {
    setSnackbarOpen(false);
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

    if (hasError) return;

    try {
      const response = await fetch("https://localhost:7262/api/Account/Login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          email: data.email,
          password: data.password,
        }),
      });

      const result = await response.json();

      if (response.ok) {
        localStorage.setItem("token", result.token);
        const expiration = new Date();
        expiration.setMinutes(expiration.getMinutes() + 30);
        localStorage.setItem("expiration", expiration.toISOString());
        localStorage.setItem("email", result.email);
        localStorage.setItem("name", result.personName);
        localStorage.setItem("userId", result.id);
        setLoginError(false);
        navigate("/user/home");
      } else {
        setLoginError(true);
        setLoginFailed(result.detail);
      }
    } catch (error) {
      console.error("Error logging in:", error);
    }
  }

  return (
    <div
      className="container"
      ref={containerRef}
      onMouseEnter={handleMouseEnter}
    >
      <div className="top"></div>
      <div className="bottom"></div>
      <form className="center" onSubmit={handleSubmit}>
        <h2>Login</h2>

        <div className="input-wrapper">
          <Input2
            label="Email"
            type="email"
            name="email"
            error={emailNotValid}
            errorText="Please enter a valid email address"
          />
        </div>

        <div className="input-wrapper">
          <Input2
            label="Password"
            type="password"
            name="password"
            error={passwordEmpty}
            errorText="Please enter a valid password"
          />
        </div>

        {loginError && <p className="err">{loginFailed}</p>}

        <Button
          variant="contained"
          color="success"
          type="submit"
          sx={{ marginTop: "20px" }}
        >
          Login
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
          Don’t have an account? <Link to="/register">Register here</Link>
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
