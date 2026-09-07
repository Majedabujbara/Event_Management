import { Button } from "@mui/material";
import Input2 from "./Input2";
import { useNavigate, useParams } from "react-router-dom";
import { useState } from "react";
import TextA from "./TextA";

function formatHtml(html) {
  if (!html) return;

  const formatted = html
    .replace(/></g, ">\n<")
    .trim();

  return formatted;
}

export default function OrgForm({ org, method }) {
  const navigate = useNavigate();
  const params = useParams();

  const [orgEmpty, setOrgEmpty] = useState(false);
  const [collegeEmpty, setCollegeEmpty] = useState(false);
  const [numberError, setNumberError] = useState("");
  const [emailError, setEmailError] = useState("");
  const [desEmpty, setDesEmpty] = useState(false);
  const [photoEmpty, setPhotoEmpty] = useState(false);

  async function handleSubmit(e) {
    e.preventDefault();

    const fd = new FormData(e.target);
    const data = Object.fromEntries(fd.entries());

    let hasError = false;

    const jordanPhoneRegex = /^07[7-9]\d{7}$/;

    if (!data.ContactNumber.trim()) {
      setNumberError("Please fill this field");
      hasError = true;
    } else if (!jordanPhoneRegex.test(data.ContactNumber.trim())) {
      setNumberError("Please enter a valid Jordanian phone number");
      hasError = true;
    } else {
      setNumberError("");
    }

    if (!data.Name.trim()) {
      setOrgEmpty(true);
      hasError = true;
    } else {
      setOrgEmpty(false);
    }

    if (!data.Description.trim()) {
      setDesEmpty(true);
      hasError = true;
    } else {
      setDesEmpty(false);
    }

    if (!data.College.trim()) {
      setCollegeEmpty(true);
      hasError = true;
    } else {
      setCollegeEmpty(false);
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.(com|net|org|edu|gov|mil|jo|edu\.jo)$/i;

    if (!data.Email.trim()) {
      setEmailError("Please fill this field");
      hasError = true;
    } else if (!emailRegex.test(data.Email.trim())) {
      setEmailError("Please enter a valid email address");
      hasError = true;
    } else {
      setEmailError("");
    }

    const orgPhotoFile = fd.get("Logo");

    if (method === "POST" && (!orgPhotoFile || orgPhotoFile.size === 0)) {
      setPhotoEmpty(true);
      hasError = true;
    } else {
      setPhotoEmpty(false);
    }

    if (hasError) return;

    let url = "https://localhost:7262/api/Organizations";

    if (method === "PATCH") {
      url = `https://localhost:7262/api/Organizations?Id=${params.id}`;
    }

    const response = await fetch(url, {
      method,
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
        // Don't manually set Content-Type when using FormData
      },
      body: fd,
    });

    if (!response.ok) {
      console.log("newOrgFail");
    } else {
      navigate("/user/organizations");
    }
  }

  return (
    <div className="org-form-container">
      <h2 className="org-form-header">
        {method === "POST" ? "Create Organization" : "Edit Organization"}
      </h2>
      <form onSubmit={handleSubmit} className="org-form">
        <div className="form-grid">
          <div className="form-column">
            <Input2
              label="Organization Name"
              type="text"
              name="Name"
              defaultValue={org?.name || ""}
              error={orgEmpty}
              errorText="Please fill this field"
              fullWidth
            />
            <Input2
              label="College"
              type="text"
              name="College"
              defaultValue={org?.college || ""}
              error={collegeEmpty}
              errorText="Please fill this field"
              fullWidth
            />
            <Input2
              label="Contact Number"
              type="tel"
              name="ContactNumber"
              defaultValue={org?.contactNumber || ""}
              error={!!numberError}
              errorText={numberError}
              fullWidth
            />
            <Input2
              label="Email"
              type="email"
              name="Email"
              defaultValue={org?.email || ""}
              error={!!emailError}
              errorText={emailError}
              fullWidth
            />
          </div>

          <div className="form-column">
            <TextA
              label="Description"
              name="Description"
              defaultValue={formatHtml(org?.description) || ""}
              error={desEmpty}
              errorText="Please fill this field"
            />
            <div className="file-input-group">
              <Input2
                InputLabelProps={{ shrink: true }}
                label="Organization Logo"
                type="file"
                name="Logo"
                error={photoEmpty}
                errorText="Please pick an image"
                inputProps={{ accept: "image/*" }}
                fullWidth
              />
            </div>
          </div>
        </div>

        <Input2
          style={{ display: "none" }}
          name="LogoUrl"
          defaultValue={org?.logoUrl || ""}
        />

        <Button
          sx={{ marginTop: "15px" }}
          variant="contained"
          type="submit"
          className="submit-btn"
          color="success"
        >
          {method === "POST" ? "Create Organization" : "Update Organization"}
        </Button>
      </form>
    </div>
  );
}
