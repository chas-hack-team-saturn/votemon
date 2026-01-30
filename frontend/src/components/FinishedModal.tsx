import React from "react";
import styles from "./FinishedModal.module.css";

interface FinishedModalProps {
  onClose: () => void; // Typ för onClose-prop
}

const FinishedModal: React.FC<FinishedModalProps> = ({ onClose }) => {
  return (
    <div>
      <div className={styles.modal}>
        <h2>Du är färdig!</h2>
      </div>
    </div>
  );
};

export default FinishedModal;
