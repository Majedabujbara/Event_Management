import { Button } from "@mui/material";
import Input2 from "./Input2";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getOrgs } from "../utility/apiGetCalls";
import TextA from "./TextA";

function formatHtml(html) {
  if (!html) return;

  const formatted = html
    .replace(/></g, ">\n<")
    .trim();

  return formatted;
}

export default function EventForm({ event, method }) {
  const navigate = useNavigate();
  const params = useParams();
  const [org, setOrg] = useState([]);
  const [selectedOrgId, setSelectedOrgId] = useState("");
  const [minDateTime, setMinDateTime] = useState("");

  const [enameempty, setEnameempty] = useState(false);
  const [edesempty, setEdesempt] = useState(false);
  const [sempty, setSempty] = useState(false);
  const [eempty, setEempty] = useState(false);
  const [photoEmpty, setPhotoEmpty] = useState(false);
  const [orgError, setOrgError] = useState(false);
  const [timeError, setTimeError] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();

    const fd = new FormData(e.target);
    const data = Object.fromEntries(fd.entries());

    let hasError = false;

    if (!data.Name.trim()) {
      setEnameempty(true);
      hasError = true;
    } else {
      setEnameempty(false);
    }

    if (!data.Description.trim()) {
      setEdesempt(true);
      hasError = true;
    } else {
      setEdesempt(false);
    }

    if (!data.EndTime.trim()) {
      setEempty(true);
      hasError = true;
    } else {
      setEempty(false);
    }

    if (!data.StartTime.trim()) {
      setSempty(true);
      hasError = true;
    } else {
      setSempty(false);
    }

    const eventPhotoFile = fd.get("EventPhoto");

    if (method === "POST" && (!eventPhotoFile || eventPhotoFile.size === 0)) {
      setPhotoEmpty(true);
      hasError = true;
    } else {
      setPhotoEmpty(false);
    }

    const startTime = new Date(data.StartTime);
    const endTime = new Date(data.EndTime);

    if (startTime >= endTime) {
      setTimeError("End time must be after start time.");
      hasError = true;
    } else {
      setTimeError("");
    }

    if (!selectedOrgId) {
      setOrgError(true);
      hasError = true;
    } else {
      setOrgError(false);
    }

    if (hasError) return;

    fd.set("organizationID", selectedOrgId);

    let url = "https://localhost:7262/api/Events";

    if (method === "PATCH") {
      url = `https://localhost:7262/api/Events?Id=${params.id}`;
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
      console.log(await response.json());
    } else {
      navigate("/user/events");
    }
  }

  useEffect(() => {
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, "0");
    const day = String(now.getDate()).padStart(2, "0");
    const hours = String(now.getHours()).padStart(2, "0");
    const minutes = String(now.getMinutes()).padStart(2, "0");

    const formattedNow = `${year}-${month}-${day}T${hours}:${minutes}`;
    setMinDateTime(formattedNow);
  }, []);

  useEffect(() => {
    async function fetchAndSetOrgs() {
      const data = await getOrgs();
      if (data) {
        setOrg(data);
      }
    }

    fetchAndSetOrgs();
  }, []);

  useEffect(() => {
    if (event?.organizationID) {
      setSelectedOrgId(event.organizationID);
    }
  }, [event]);

  return (
    <div className="event-form-container">
      <h2 className="event-form-header">
        {method === "POST" ? "Create Event" : "Edit Event"}
      </h2>
      <form onSubmit={handleSubmit} className="event-form">
        {timeError && (
          <p align="center" style={{ color: "red", fontSize: "20px" }}>
            {timeError}
          </p>
        )}
        <div className="form-grid">
          <div className="form-column">
            <Input2
              label="Event Name"
              type="text"
              name="Name"
              defaultValue={event?.name || ""}
              error={enameempty}
              errorText="Please fill this field"
              fullWidth
            />
            <TextA
              label="Description"
              name="Description"
              defaultValue={formatHtml(event?.description) || ""}
              error={edesempty}
              errorText="Please fill this field"
            />
          </div>

          <div className="form-column">
            <Input2
              label="Start Time"
              type="datetime-local"
              name="StartTime"
              InputLabelProps={{ shrink: true }}
              defaultValue={event?.startTime.split(".")[0] || ""}
              inputProps={{ min: minDateTime }}
              fullWidth
              error={sempty}
              errorText="Please fill this field"
            />
            <Input2
              label="End Time"
              type="datetime-local"
              name="EndTime"
              InputLabelProps={{ shrink: true }}
              defaultValue={event?.endTime.split(".")[0] || ""}
              inputProps={{ min: minDateTime }}
              fullWidth
              error={eempty}
              errorText="Please fill this field"
            />

            <div className="form-control">
              <label>Organization</label>
              <select
                value={selectedOrgId}
                onChange={(e) => setSelectedOrgId(e.target.value)}
                className="org-select"
              >
                <option value="">Select an organization</option>
                {org.map((o) => (
                  <option key={o.organizationID} value={o.organizationID}>
                    {o.name}
                  </option>
                ))}
              </select>
              {orgError && (
                <p style={{ color: "red", margin: "2px" }}>
                  Please select an organization
                </p>
              )}
            </div>

            <Input2
              label="Event Photo"
              InputLabelProps={{ shrink: true }}
              type="file"
              name="EventPhoto"
              error={photoEmpty}
              errorText="Please pick an image"
              inputProps={{ accept: "image/*" }}
              fullWidth
            />
          </div>
        </div>

        <Input2
          style={{ display: "none" }}
          name="PhotoUrl"
          defaultValue={event?.photoUrl || ""}
        />

        <Button
          sx={{ marginTop: "20px" }}
          variant="contained"
          type="submit"
          className="submit-btn"
          color="success"
        >
          {method === "POST" ? "Create Event" : "Update Event"}
        </Button>
      </form>
    </div>
  );
}
