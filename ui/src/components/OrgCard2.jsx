import { Link } from "react-router-dom";
import { Typography } from "@mui/material";

export default function OrgCard2({ name, image, to }) {
  return (
    <Link to={to} className="org-card2-link">
      <div className="org-card2">
        <img
          src={
            image ? `https://localhost:7262/${image}` : "/Images/emptyPhoto.png"
          }
          alt={name}
          className="org-logo"
        />
        <div className="org-card2-content">
          <Typography variant="subtitle1" className="org-card2-title">
            <span>{name}</span>
          </Typography>
        </div>
      </div>
    </Link>
  );
}
