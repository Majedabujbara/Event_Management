import { format, parseISO, isBefore } from "date-fns";
import { Button } from "@mui/material";
import InfoIcon from "@mui/icons-material/Info";
import EventIcon from "@mui/icons-material/Event";
import GroupsIcon from "@mui/icons-material/Groups";
import { useEffect, useState } from "react";

export default function Card({ name, startDate, endDate, orgId }) {
  const [org, setOrg] = useState({ name: "..." });
  const [status, setStatus] = useState("");

  useEffect(() => {
    async function fetchOrgById() {
      try {
        const response = await fetch(
          `https://localhost:7262/api/Organizations/${orgId}`
        );
        if (response.ok) {
          const json = await response.json();
          setOrg(json);
        }
      } catch (err) {
        console.error("Failed to fetch organization:", err);
      }
    }

    fetchOrgById();
  }, [orgId]);

  useEffect(() => {
    if (startDate && endDate) {
      const start = parseISO(startDate);
      const end = parseISO(endDate);
      const now = new Date();

      if (isBefore(now, start)) {
        setStatus("Upcoming");
      } else if (isBefore(now, end)) {
        setStatus("Ongoing");
      } else {
        setStatus("Ended");
      }
    } else {
      setStatus("Unknown");
    }
  }, [startDate, endDate]);

  const formattedDate = startDate
    ? format(parseISO(startDate), "EEE, MMM d, yyyy 'at' h:mm a")
    : "N/A";

  const dayOfMonth = startDate ? format(parseISO(startDate), "d") : "--";
  const month = startDate ? format(parseISO(startDate), "MMM") : "---";

  return (
    <div className="event-card-modern">
      <div className="event-card-left">
        <span className="day">{dayOfMonth}</span>
        <span className="month">{month}</span>
      </div>
      <div className="event-card-right">
        <h3 className="event-title-modern">{name}</h3>

        <div className="event-meta-modern">
          <div className="info">
            <EventIcon fontSize="small" />
            <span>{formattedDate}</span>
          </div>
          <div className="info">
            <GroupsIcon fontSize="small" />
            <span>{org.name}</span>
          </div>
          <div className="info">
            <InfoIcon fontSize="small" />
            <span className={`event-status ${status.toLowerCase()}`}>
              {status}
            </span>
          </div>
        </div>

        <Button variant="contained" className="event-btn-modern">
          View Details
        </Button>
      </div>
    </div>
  );
}
