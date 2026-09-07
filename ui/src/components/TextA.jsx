import classes from "./TextA.module.css";

export default function TextA({ label, error, errorText, ...props }) {
  return (
    <div style={{ display: "flex", flexDirection: "column" }}>
      <label className={`${classes.label} ${error ? classes.error : ""}`}>
        {label}
      </label>
      <textarea
        rows="5"
        className={`${classes.textarea} ${error ? classes.textareaError : ""}`}
        {...props}
      />
      {error && <p style={{ margin: "0 15px", color: "red" }}>{errorText}</p>}
    </div>
  );
}
