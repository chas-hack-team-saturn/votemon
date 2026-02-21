import React from "react";
import styles from "./FinishedModal.module.css";

interface FinishedModalProps {
  onClose: () => void; // Typ för onClose-prop
}

const FinishedModal: React.FC<FinishedModalProps> = ({ onClose }) => {
  return (
    <div className={styles["modal-overlay"]}>
      <div className={styles.modal}>
        <h2>Du är färdig!</h2>
        <button
          onClick={onClose}
          style={{ marginTop: "20px", padding: "10px 20px", cursor: "pointer" }}
        >
          Stäng
        </button>
      </div>
    </div>
  );
};

export default FinishedModal;
