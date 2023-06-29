import { ArrowBackRounded, ArrowForwardRounded } from '@mui/icons-material/';
import { Box, IconButton, Slide } from '@mui/material';
import { alpha, hexToRgb } from '@mui/material/styles';
import { useRef, useState } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW, CUSTOM_SCROLLBAR } from '../../constants';

const images = [
  'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
  'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
  'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
  'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
  'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
  'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
  'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
  'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
];

export default function MediaCarousel2() {
  const [activeStep, setActiveStep] = useState(0);
  const slider = useRef<HTMLDivElement>(null);

  const handleStepChange = (step: number) => {
    setActiveStep(step);

    if (step === 0) {
      slider && slider.current && (slider.current.scrollLeft = 0);
    } else {
      slider && slider.current && (slider.current.scrollLeft += 180);
    }
  };

  const handleBackClick = (event: React.MouseEvent<HTMLElement>) => {
    slider && slider.current && (slider.current.scrollLeft -= 180);
  };

  const handleForwardClick = (event: React.MouseEvent<HTMLElement>) => {
    slider && slider.current && (slider.current.scrollLeft += 180);
  };

  return (
    <Box>
      {/* <AutoPlaySwipeableViews
        index={activeStep}
        onChangeIndex={handleStepChange}
        enableMouseEvents
        interval={2000} // Interval between slides (in milliseconds)
        style={{ height: 'auto', width: 500 }}
      >
        {images.map((image, index) => (
          <Box
            key={index}
            component="img"
            sx={{
              borderRadius: '10px',
              overflow: 'hidden'
            }}
            src={image}
            onClick={(e) => handleStepChange(index)}
          />
        ))}
      </AutoPlaySwipeableViews> */}

      <Box
        ref={slider}
        sx={{
          width: 800,
          height: 500,
          display: 'flex',
          flexWrap: 'nowrap',
          overflowX: 'hidden',
          borderRadius: '10px',
          scrollSnapType: 'x mandatory',
          WebkitOverflowScrolling: 'touch',
          boxShadow: COMPONENT_SHADOW,
        }}
      >
        {images.map((image, index) => (
          <Slide key={index} direction="left" in={activeStep === index} mountOnEnter unmountOnExit>
            <Box
              component="img"
              sx={{
                minWidth: '100%',
              }}
              src={image}
            />
          </Slide>
        ))}
        <Slide direction="left" in={activeStep === images.length} mountOnEnter unmountOnExit>
          <iframe
            title="video"
            style={{
              minWidth: '100%',
              border: 0,
            }}
            src="https://www.youtube.com/embed/tgbNymZ7vqY"
          >
            /
          </iframe>
        </Slide>
      </Box>
      <Box
        sx={{
          position: 'relative',
          width: 800,
          display: 'flex',
          minHeight: '100px',
          borderRadius: '10px',
          overflow: 'hidden',
          mt: 2,
        }}
      >
        <BoxFlexCenter
          sx={{
            position: 'absolute',
            width: '100%',
            height: '100%',
            zIndex: 1,
            pointerEvents: 'none',
            background: (theme) =>
              `linear-gradient(90deg, ${hexToRgb(theme.palette.background.default)} 0%, 
              ${alpha(theme.palette.background.default, 0.9)} 5%, 
              rgba(255,255,255,0) 15%, 
              rgba(255,255,255,0) 85%, 
              ${alpha(theme.palette.background.default, 0.9)} 95%, 
              ${hexToRgb(theme.palette.background.default)} 100%)`,
          }}
        >
          <IconButton
            onClick={handleBackClick}
            sx={{
              width: '30px',
              height: '30px',
              cursor: 'pointer',
              pointerEvents: 'auto',
              ml: 2,
            }}
          >
            <ArrowBackRounded />
          </IconButton>
          <IconButton
            onClick={handleForwardClick}
            sx={{
              width: '30px',
              height: '30px',
              cursor: 'pointer',
              pointerEvents: 'auto',
              mr: 2,
            }}
          >
            <ArrowForwardRounded />
          </IconButton>
        </BoxFlexCenter>

        <Box
          ref={slider}
          sx={{
            display: 'flex',
            flexWrap: 'nowrap',
            overflowX: 'auto',
            scrollSnapType: 'x mandatory',
            WebkitOverflowScrolling: 'touch',
            '&::-webkit-scrollbar': {
              height: '6px',
            },
            ...CUSTOM_SCROLLBAR,
          }}
        >
          {images.map((image, index) => (
            <Box
              component="img"
              key={index}
              sx={{
                maxWidth: '180px',
                maxHeight: '120px',
                scrollSnapAlign: 'center',
                cursor: 'pointer',
                margin: '5px',
                borderRadius: '6px',
                boxShadow: COMPONENT_SHADOW,
              }}
              src={image}
              onClick={(e) => handleStepChange(index)}
            />
          ))}
          <Box
            sx={{
              scrollSnapAlign: 'center',
              cursor: 'pointer',
              margin: '5px',
              borderRadius: '6px',
            }}
            onClick={(e) => handleStepChange(images.length)}
          >
            <iframe
              title="video"
              style={{
                maxWidth: '180px',
                maxHeight: '120px',
                pointerEvents: 'none',
                border: 0,
                boxShadow: COMPONENT_SHADOW,
              }}
              src="https://www.youtube.com/embed/tgbNymZ7vqY"
            ></iframe>
          </Box>
        </Box>
      </Box>
    </Box>
  );
}
