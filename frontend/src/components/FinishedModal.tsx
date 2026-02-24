import React, { useState, useEffect } from "react";
import styles from "./FinishedModal.module.css";
import { storage } from "../utils/storage";

interface FinishedModalProps {
  onClose: () => void;
}

const FinishedModal: React.FC<FinishedModalProps> = ({ onClose }) => {
  const [timeLeft, setTimeLeft] = useState(storage.getTimeToMidnight());

  useEffect(() => {
    const timer = setInterval(() => {
      setTimeLeft(storage.getTimeToMidnight());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  const formatTime = (time: {
    hours: number;
    minutes: number;
    seconds: number;
  }) => {
    return `${time.hours.toString().padStart(2, "0")}:${time.minutes
      .toString()
      .padStart(2, "0")}:${time.seconds.toString().padStart(2, "0")}`;
  };

  return (
    <div className={styles["modal-overlay"]}>
      <div className={styles.modal}>
        <h1 className={styles.title}>Voting complete!</h1>
        <div className={styles.timerContainer}>
          <p className={styles.timerLabel}>The daily vote refreshes in:</p>
          <p className={styles.timerValue}>{formatTime(timeLeft)}</p>
        </div>
        <button className={styles.closeButton} onClick={onClose}>
          Close (For testing)
        </button>
      </div>
    </div>
  );
};

export default FinishedModal;
