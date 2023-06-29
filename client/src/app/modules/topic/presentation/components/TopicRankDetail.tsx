import { IonIcon } from '@ionic/react';
import { WorkspacePremiumRounded } from '@mui/icons-material';
import {
  IconButton,
  Paper,
  PaperProps,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  TextField,
} from '@mui/material';
import { checkmarkOutline, closeCircleOutline, createOutline } from 'ionicons/icons';
import { useState } from 'react';
import { RankAward } from '~/app/modules/rankAward/domain/models/RankAward';
import { SpecificAward } from '~/app/modules/rankAward/domain/models/SpecificAward';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import {
  CustomTableHead,
  CustomTableRow,
} from '~/app/modules/shared/presentation/components/CustomTable';

type Props = PaperProps & {
  primaryColor: string;
  rankAwardList: RankAward[];
};

// function createData(award: string, certificate: boolean) {
//   return { award, certificate };
// }

// const rows = [
//   createData('2.000.000 đồng', true),
//   createData('1.500.000 đồng', true),
//   createData('1.000.000 đồng', true),
//   createData('500.000 đồng', true),
// ];

export default function TopicRankDetail({ primaryColor, rankAwardList }: Props) {
  const [rankAwards, setRankAwards] = useState<RankAward[]>(rankAwardList);
  const [prize, setPrize] = useState(0);
  const [certificate, setCertificate] = useState('Có giấy khen');
  const [editSpecific, setEditSpecific] = useState<SpecificAward | null>(null);
  // const [editCertificate, setEditCertificate] = useState('');

  const resetInfo = () => {
    setEditSpecific(null);
    setPrize(0);
  };

  const checkEmpty = (rankAwards: RankAward[]) => {
    return rankAwards.map((award) => {
      const newSpecificAwards = award.specificAwards.filter(
        (s) => s.bonusPrize > 0 || (s === editSpecific && s.bonusPrize > 0)
      );

      return { ...award, specificAwards: newSpecificAwards };
    });
  };

  const updateInfo = () => {
    const newRankAwards = rankAwards.map((award) => {
      const matchedSpecificAwards = award.specificAwards.find((s) => s === editSpecific);

      if (!matchedSpecificAwards) return award;

      const newSpecific: SpecificAward = {
        id: editSpecific?.id ?? 0,
        year: editSpecific?.year ?? 0,
        rankAwardId: editSpecific?.rankAwardId ?? 0,
        passed: editSpecific?.passed ?? false,
        bonusPrize: +prize,
      };

      return { ...award, specificAwards: [newSpecific] };
    });

    setRankAwards(checkEmpty(newRankAwards));
    resetInfo();
  };

  return (
    <Paper elevation={0} sx={{ boxShadow: COMPONENT_SHADOW, p: 2 }}>
      <TableContainer>
        <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
          <CustomTableHead
            sx={{
              '& .MuiTableCell-root': {
                color: primaryColor,
              },
            }}
          >
            <TableRow>
              <TableCell align="center">Thứ hạng</TableCell>
              <TableCell align="center">Số tiền</TableCell>
              <TableCell align="center">Giấy khen</TableCell>
              <TableCell align="center">Chỉnh sửa</TableCell>
            </TableRow>
          </CustomTableHead>
          <TableBody>
            {rankAwards.map((award, i) => {
              const specificAward = award.specificAwards[0];

              return (
                specificAward && (
                  <CustomTableRow
                    key={i}
                    sx={{
                      '&:not(:last-child)': {
                        borderBottom: '1px dashed #ccc',
                      },
                      '& .MuiTableCell-root': {
                        fontWeight: 700,
                        color: 'darkslategray',
                      },
                    }}
                  >
                    <TableCell align="center">
                      <WorkspacePremiumRounded />
                    </TableCell>
                    <TableCell align="center">
                      {editSpecific === specificAward ? (
                        <TextField
                          sx={{
                            width: 220,
                            backgroundColor: 'white',
                            m: 0,
                            ml: 1,
                            '& .MuiInputBase-input': {
                              py: 1,
                            },
                          }}
                          type="number"
                          placeholder="Nhập số tiền thưởng"
                          inputProps={{
                            min: 0,
                            max: 100000000,
                          }}
                          value={prize}
                          onChange={(e) => setPrize(+e.target.value)}
                        />
                      ) : (
                        specificAward.bonusPrize
                          .toLocaleString('en-US', { minimumFractionDigits: 0 })
                          .replace(/,/g, '.') + ' đồng'
                      )}
                    </TableCell>
                    <TableCell align="center">{certificate}</TableCell>
                    <TableCell align="center">
                      {editSpecific === specificAward ? (
                        <IconButton
                          color="primary"
                          onClick={(e) => {
                            updateInfo();
                          }}
                        >
                          <IonIcon icon={checkmarkOutline} />
                        </IconButton>
                      ) : (
                        <IconButton
                          color="info"
                          onClick={(e) => {
                            setPrize(specificAward.bonusPrize);
                            setEditSpecific(specificAward);
                          }}
                        >
                          <IonIcon icon={createOutline} />
                        </IconButton>
                      )}
                      <IconButton
                        color="error"
                        onClick={(e) => {
                          if (editSpecific && editSpecific !== specificAward) return;

                          if (editSpecific) {
                            resetInfo();
                            setRankAwards(checkEmpty(rankAwards));
                            return;
                          }

                          // remove award
                          
                          // const newRankAwards = rankAwards.map((award) => {
                          //   const newSpecificAwards = award.specificAwards.filter(
                          //     (s) => s !== specificAward
                          //   );
                          //   return { ...award, specificAwards: newSpecificAwards };
                          // });
                          // resetInfo();
                          // setRankAwards(checkEmpty(newRankAwards));
                        }}
                      >
                        <IonIcon icon={closeCircleOutline} />
                      </IconButton>
                    </TableCell>
                  </CustomTableRow>
                )
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Paper>
  );
}
