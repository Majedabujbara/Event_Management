import { Button, Typography, Divider } from "@mui/material";
import { useNavigate, useParams, useRouteLoaderData } from "react-router-dom";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

export default function OrgDetails() {
  const params = useParams();
  const navigate = useNavigate();
  const privateOrg = useRouteLoaderData("private-org-details");
  const publicOrg = useRouteLoaderData("public-org-details");
  const org = privateOrg || publicOrg;

  async function handleDelete() {
    const response = await fetch(
      `https://localhost:7262/api/Organizations/${params.id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );

    if (!response.ok) {
      console.log("delete failed");
    } else {
      navigate("/user/organizations");
    }
  }

  return (
    <div className="org-details-page">
      {org && (
        <div className="org-details-container">
          {localStorage.getItem("email") === "admin@example.com" && (
            <div className="admin-actions">
              <Button
                variant="contained"
                color="primary"
                onClick={() => navigate("edit")}
                className="edit-btn"
                startIcon={<EditIcon />}
              >
                Edit
              </Button>
              <Button
                variant="contained"
                color="error"
                onClick={handleDelete}
                className="delete-btn"
                startIcon={<DeleteIcon />}
              >
                Delete
              </Button>
            </div>
          )}

          <div className="org-header">
            <div className="logo-container">
              <img
                src={
                  org.logoUrl
                    ? `https://localhost:7262/${org.logoUrl}`
                    : "/Images/emptyPhoto.png"
                }
                alt={org.name}
                className="org-logo"
              />
            </div>
            <div className="org-info">
              <Typography variant="h3" className="org-name">
                {org.name}
              </Typography>
              <Typography variant="subtitle1" className="org-college">
                {org.college}
              </Typography>
            </div>
          </div>

          <Divider className="divider" />

          <div className="org-section">
            <Typography variant="h4" className="section-title">
              About
            </Typography>
            <Typography
              variant="body1"
              className="org-description"
              component="div"
              dangerouslySetInnerHTML={{ __html: org.description }}
            />
          </div>

          <Divider className="divider" />

          <div className="org-section">
            <Typography variant="h4" className="section-title">
              Contact
            </Typography>
            <div className="contact-info">
              <div className="contact-item">
                <Typography variant="body1" className="contact-label">
                  Email:
                </Typography>
                <Typography variant="body1" className="contact-value">
                  {org.email}
                </Typography>
              </div>
              <div className="contact-item">
                <Typography variant="body1" className="contact-label">
                  Phone:
                </Typography>
                <Typography variant="body1" className="contact-value">
                  {org.contactNumber}
                </Typography>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export async function loader({ params }) {
  const response = await fetch(
    `https://localhost:7262/api/Organizations/${params.id}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
        "Content-Type": "application/json",
      },
    }
  );

  if (!response.ok) {
    throw new Response("Failed to fetch organization", {
      status: response.status,
    });
  }

  const resData = await response.json();
  return resData;
}
