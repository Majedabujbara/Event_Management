import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function PageNotFound() {
  const navigate = useNavigate();

  const handleRedirect = () => {
    const token = localStorage.getItem("token");
    if (token) {
      navigate("/user/home");
    } else {
      navigate("/");
    }
  };

  return (
    <div
      style={{
        textAlign: "center",
        marginTop: "50px",
        backgroundColor: "rgb(241, 241, 241)",
        padding: "30px",
      }}
    >
      <h1 style={{ color: "green" }}>Sorry!</h1>
      <h6 style={{ marginBottom: "30px" }}>
        We can't find the page you are looking for
      </h6>
      <Button variant="contained" color="success" onClick={handleRedirect}>
        Go to Main page
      </Button>
    </div>
  );
}
