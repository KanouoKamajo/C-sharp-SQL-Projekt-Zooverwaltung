console.log("JS geladen – bereit für Action!");

// Navigation responsive umschalten
function toggleMenu() {
  document.getElementById("nav-links").classList.toggle("show");
}

// Sektionen ein-/ausblenden bei Button-Klick
function toggleSection(id) {
  const section = document.getElementById(id);
  if (section.style.display === "none" || section.style.display === "") {
    section.style.display = "block";
    section.scrollIntoView({ behavior: "smooth" });
  } else {
    section.style.display = "none";
  }
}

// Aktiver Link in Navbar beim Scrollen markieren
const sections = document.querySelectorAll("section");
const navLinks = document.querySelectorAll(".nav-links a");

window.addEventListener("scroll", () => {
  let current = "";
  sections.forEach((section) => {
    const sectionTop = section.offsetTop - 100;
    if (scrollY >= sectionTop) {
      current = section.getAttribute("id");
    }
  });

  navLinks.forEach((link) => {
    link.classList.remove("active");
    if (link.getAttribute("href").includes(current)) {
      link.classList.add("active");
    }
  });
});
// Nur diese Sektion anzeigen – andere ausblenden
function showExclusiveSection(id) {
    const allSections = ['about-vision', 'skills', 'certificates', 'references', 'feedback', 'long-about'];
    allSections.forEach(sectionId => {
      const el = document.getElementById(sectionId);
      if (el && sectionId !== id) {
        el.style.display = 'none';
      }
    });
  
    const target = document.getElementById(id);
    if (target) {
      target.style.display = 'block';
      target.scrollIntoView({ behavior: 'smooth' });
    }
  }
  
  