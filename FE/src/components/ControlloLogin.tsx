import React, { useState, useEffect } from "react";
import { getImage } from "../api/Image";
import LoginForm from "./LoginForm";
import { useNavigate } from "react-router-dom";

type Image = {
  fileName: string;
  contentType: string;
};

export function UploadImage() {
  const [images, setImages] = useState<Image[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchImage = async () => {
      try {
        const response = await getImage();
        setImages(response.data); // TypeScript sa che sono Image[]
      } catch (error) {
        console.error("Errore nel recupero dell'immagine:", error);
      }
    };

    fetchImage();
  }, []);

  const handleLoginSubmit = async (email: string, password: string) => {
    try {
      const response = await fetch("/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (response.ok) {
        localStorage.setItem("token", data.token);
        navigate("/table");
      } else {
        console.error("Errore login:", data.message);
      }
    } catch (error) {
      console.error("Errore login:", error);
    }
  };

  return (
    <div>
      <div>
        {images.map((img) => (
          <div key={img.fileName}>
            <p>{img.fileName}</p>
            <p>{img.contentType}</p>
          </div>
        ))}
      </div>

      <div>
        <LoginForm onSubmit={handleLoginSubmit} />
      </div>
    </div>
  );
}
