import { Link } from "react-router-dom";
import { Button, Typography } from "@mui/material";
import GroupsIcon from '@mui/icons-material/Groups';

export default function OrgCard({ name, image, to }) {
  return (
    <div className="org-card">
      <div className="org-card-content">
        <img
          src={image ? `https://localhost:7262/${image}` : "/Images/emptyPhoto.png"}
          alt={name}
          className="org-logo"
        />
        <Typography variant="subtitle1" className="org-name">
          {name}
        </Typography>
      </div>
      <Button
        component={Link}
        to={to}
        variant="outlined"
        size="small"
        color="success"
        startIcon={<GroupsIcon />}
        className="view-details-btn"
      >
        View Details
      </Button>
    </div>
  );
}